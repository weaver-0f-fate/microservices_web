import { useCallback } from "react"


export const useUpdateEvent = () => {

    return useCallback(
        (eventUuid: string, path: string, value: any) => 
            fetch(`http://localhost:7203/api/events/${eventUuid}`, {
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