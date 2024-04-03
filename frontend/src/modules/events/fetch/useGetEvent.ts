import { useCallback } from "react"
import { usePublicFetch } from "../../../shared/fetch/use-fetch";

export const useGetEvent = () => {
    const fetch = usePublicFetch();
    
    return useCallback(
        (eventUuid: string) : Promise<EventResponse> => 
            fetch(`http://localhost:7201/api/events/${eventUuid}`, {
                method: 'GET',
                nokNotification: "test"
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