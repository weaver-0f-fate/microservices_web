import { useCallback } from "react"
import { usePublicFetch } from "../../../shared/fetch/use-fetch";

export const useSearchEvents = () => {
    const fetch = usePublicFetch();

    return useCallback(
        (searchString?: string) : Promise<any> => 
            fetch(`/events/search?searchString=${searchString}`, {
                method: 'GET', 
        }), [])
}

export interface SearchEvent {
    uuid: string,
    title: string,
    description: string,
    place: string
}