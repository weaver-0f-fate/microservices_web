import { useNotify } from '../hooks/notify';
import { useCallback } from 'react';
import { FetchResponseTypes } from './fetch-response';
import qs from 'qs';
import { useAuth } from 'react-oidc-context';
import { Severity } from '../hooks/notify/severity';

const STATUS_CODE_VALIDATION_FAILED = 422;
const STATUS_CODE_INTERNAL_SERVER_ERROR = 500;
const STATUS_CODE_UNAUTHORIZED = 401;
const STATUS_CODE_FORBIDDEN = 403;
const STATUS_CODE_BAD_GATEWAY = 502;
const STATUS_CODE_METHOD_NOT_ALLOWED = 405;
const STATUS_CODE_NOT_FOUND = 404;
const STATUS_CODE_BAD_REQUEST = 400;

const usePublicFetch = () => {
    const fetch = useFetch();

    return useCallback((path: string, options: FetchOptions) => {
        return fetch(path, options)
            .catch((error) => {
                return Promise.reject(error);
            });
    }, [fetch]);
}

const useSecureFetch = () => {
    const fetch = useFetch();

    //Selleks, et vältida re-renderdamist eemaldame igasuguse sõltuvuse välistest state'idest
    //ja võtame access tokeni otse session storeist.
    const accessToken = () => {
        for (let i = 0; i < localStorage.length; i++) {
            let keyName = localStorage.key(i);
            if(keyName?.startsWith("user:")){
                let json = JSON.parse(localStorage.getItem(keyName)!);
                return json.access_token
            }
        }

    }

    return useCallback(async (path: any, options = {}) => {
        return fetch(path, { ...options, headers: { Authorization: "Bearer " + accessToken()}})
            .catch((error) => {
                return Promise.reject(error);
            });

    }, [fetch]);
}

type FetchOptions = {
    method: string,
    headers?: any,
    responseType?: FetchResponseTypes,
    query?: {},
    body?: FormData | string | any,
    crossDomain?: boolean,
    nokNotification?: string,
    signal?: AbortSignal
}

declare var AbortSignal: {
    prototype: AbortSignal
    new(): AbortSignal
    abort(reason?: any): AbortSignal
    timeout(milliseconds: number): AbortSignal
}

const useFetch = () => {

    const notify = useNotify();

    return useCallback((path: any, options: any) => {
        const {
            body,
            method,
            nokNotification,
            badRequestNotification,
            query,
            responseType = FetchResponseTypes.JSON,
            headers,
            showLoader = true,
            timeout,
        } = options;

        const fetchOptions: FetchOptions = {
            query: {
                ...query
            },
            headers: {
                ...headers
            },
            responseType: responseType,
            method: method,
            crossDomain:true,
        };

        if (body instanceof FormData) {
            fetchOptions.body = body;
            fetchOptions.method = method || "POST";
        } else if (body !== undefined) {
            fetchOptions.body = JSON.stringify(body);
            fetchOptions.method = method || "POST";
            fetchOptions.headers['Content-Type'] = 'application/json';
        }

        if (!fetchOptions.method)
            fetchOptions.method = 'GET';

        if (timeout) {
            fetchOptions.signal = AbortSignal.timeout(timeout);
        }

        return fetchFromApi(path, fetchOptions, responseType)
            .then((result) => {
                return Promise.resolve(result);
            })
            .catch(async (error) => {
                let errors = [] as Array<string>;

                if (nokNotification && nokNotification.length > 0) {
                    errors.push(nokNotification);
                }

                switch (error.status) {
                    case STATUS_CODE_VALIDATION_FAILED:
                        const data = await error.json();
                        errors.push(data.errorMessages.join('. '));
                        break;
                    case STATUS_CODE_INTERNAL_SERVER_ERROR:
                        errors.push('500: Technical error');
                        break;
                    case STATUS_CODE_METHOD_NOT_ALLOWED:
                        errors.push('405: Technical error');
                        break;
                    case STATUS_CODE_FORBIDDEN:
                        const message = (await error.json()).message;
                        errors.push(message.length > 0 ? message : '403: Forbidden');
                        break;
                    case STATUS_CODE_BAD_GATEWAY:
                        errors.push('502: Bad gateway');
                        break;
                    case STATUS_CODE_NOT_FOUND:
                        errors.push('404: Not found');
                        break;
                    case STATUS_CODE_BAD_REQUEST:
                        errors.pop()
                        errors.push(badRequestNotification)
                        errors.push('400: Bad request');
                        break;
                }

                notify(errors.join('. '), Severity.Error);

                return Promise.reject(error);
            });
    }, []);
}

const fetchFromApi = (path: string, options: FetchOptions, responseType: FetchResponseTypes) => {
    return fetch(`${process.env.REACT_APP_API}${path}${options.query && Object.keys(options.query).length !== 0 ? `?${stringify(options.query)}` : ''}`, options)
        .then(responseOk)
        .then(async response => {
            try {
                switch (responseType) {
                    case FetchResponseTypes.BLOB:
                        return response.blob();
                    case FetchResponseTypes.TEXT:
                        return response.text();
                    default:
                        try {
                            const respJson = await response.json();
                            const json = respJson.data || respJson;

                            return Promise.resolve(json);
                        } catch (error) {
                            return Promise.resolve({});
                        }
                }
            } catch (error) {
                return Promise.resolve(undefined);
            }
        })
        .catch((error) => {
            return Promise.reject(error);
        })
}

const responseOk = async ( response: Response ) => (response.ok ? response : Promise.reject(response));

const stringify = (query: any) => qs.stringify(query, {
    arrayFormat: 'comma'
});

export {
    usePublicFetch,
    useSecureFetch
};