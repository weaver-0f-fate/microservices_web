import { useCallback } from "react"
import { useSecureFetch } from "../../../shared/fetch/use-fetch";


export const useUpdateEvent = () => {
    const fetch = useSecureFetch();

    return useCallback(
        (eventUuid: string, path: string, value: any) => 
            fetch(`/events/${eventUuid}`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json-patch+json' },
                body: [
                    {
                        op: 'add',
                        path: path,
                        value: value
                    }
                ],
        }), [])
}