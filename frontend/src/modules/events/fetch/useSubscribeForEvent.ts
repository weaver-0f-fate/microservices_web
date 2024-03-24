import { useCallback } from "react"


export const useSubscribeForEvent = () => {

    return useCallback(
        (eventUuid: string, subscribedEmail: string, notificationTime: Date) => 
            fetch(`http://localhost:7204/api/subscription`, {
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