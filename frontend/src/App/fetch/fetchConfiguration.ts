import { useCallback } from "react";
import { usePublicFetch } from "../../shared/fetch/use-fetch";
import { FetchResponseTypes } from "../../shared/fetch/fetch-response";

export type AppConfigurationResponse = {
    Environment?: string,
    EnvironmentDisplayName?: string,
    Version?: string,
    Keycloak: {
        AuthUrl: string,
        ClientId: string,
        Realm: string,
    },
}

export const useGetConfiguration = () => {
    const fetch = usePublicFetch();
    return useCallback(
        (): Promise<AppConfigurationResponse> => fetch(`/configuration`, {
            method: 'GET',
            responseType: FetchResponseTypes.JSON,
            nokNotification: 'System configuration request failed'
        }), [fetch]);
}