import { useCallback } from "react"
import { usePublicFetch } from "../../../shared/fetch/use-fetch";


export const useSubscribeForEvent = () => {
    const fetch = usePublicFetch();

    return useCallback(
        (eventUuid: string, subscribedEmail: string, notificationTime: Date) => 
            fetch(`http://localhost:7201/api/subscription`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    eventUuid,
                    subscribedEmail,
                    notificationTime: notificationTime.toISOString()
                }),
        }), [])
}