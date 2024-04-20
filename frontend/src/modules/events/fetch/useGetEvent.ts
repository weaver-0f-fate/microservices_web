import { useCallback } from "react"
import { useSecureFetch } from "../../../shared/fetch/use-fetch";

export const useGetEvent = () => {
    const fetch = useSecureFetch();
    
    return useCallback(
        (eventUuid: string) : Promise<EventResponse> => 
            fetch(`/events/${eventUuid}`, {
                method: 'GET',
                nokNotification: "Get event request failed"
        }), [])
}

export interface EventResponse {
    uuid: string,
    category: string,
    title: string,
    imageUrl: string,
    description: string,
    place: string,
    date: string,
    additionalInfo: string,
    recurrency?: string,
}