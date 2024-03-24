import { useCallback } from "react"

export const useSearchEvents = () => {

    return useCallback(
        (searchString?: string) : Promise<any> => 
            fetch(`http://localhost:7203/api/events/search?searchString=${searchString}`, {
                method: 'GET', 
        }), [])
}

export interface SearchEvent {
    uuid: string,
    title: string,
    description: string,
    place: string
}