import { useCallback } from "react"
import { formatTime } from "../../../shared/utilities/dateFunctions"
import { usePublicFetch, useSecureFetch } from "../../../shared/fetch/use-fetch"

export const useGetEvents = () => {
    const fetch = usePublicFetch();
    
    return useCallback(
        (category?: string, place?: string, date?: Date) : Promise<EventResponse[]> => 
            fetch(`http://localhost:7201/api/events`, {
                method: 'GET',
                query: {
                    category,
                    place,
                    time: date ? formatTime(date) : ''
                }
        }), [])
}

export interface EventResponse {
    title: string;
    rrule?: string,
    date: string;
    category: string,
    place: string,
    uuid: string;
}