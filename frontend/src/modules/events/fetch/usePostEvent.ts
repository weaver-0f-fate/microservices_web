import { Dayjs } from "dayjs"
import { useCallback } from "react"
import { usePublicFetch } from "../../../shared/fetch/use-fetch";

export interface PostEventProps {
    category: string,
    title: string,
    imageUrl: string,
    description: string,
    place: string,
    additionalInfo: string,
    date: Dayjs,
}

export const usePostEvent = () => {
    const fetch = usePublicFetch();

    return useCallback(
        (props: PostEventProps) : Promise<PostEventResponse> => 
            fetch(`http://localhost:7201/api/events`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    category: props.category,
                    title: props.title,
                    imageUrl: props.imageUrl,
                    description: props.description,
                    place: props.place,
                    additionalInfo: props.additionalInfo,
                    date: props.date.toISOString()
                }),
        }), [])
}

export interface PostEventResponse {
    uuid: string,
    category: string,
    title: string,
    place: string,
    date: string,
}