import { useCallback } from "react"
import { useSecureFetch } from "../../../shared/fetch/use-fetch";


export const useUpdateEvent = () => {
    const fetch = useSecureFetch();

    return useCallback(
        (eventUuid: string, path: string, value: any) => 
            fetch(`http://localhost:7201/api/events/${eventUuid}`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json-patch+json' },
                body: JSON.stringify([
                    {
                        op: 'add',
                        path: path,
                        value: value
                    }
                ]),
        }), [])
}