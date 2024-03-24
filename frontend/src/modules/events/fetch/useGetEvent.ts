import { useCallback } from "react"

export const useGetEvent = () => {
    return useCallback(
        (eventUuid: string) => 
            fetch(`http://localhost:7203/api/events/${eventUuid}`, {
                method: 'GET',
        }), [])
}