import { Dayjs } from "dayjs"
import { useCallback } from "react"

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

    return useCallback(
        (props: PostEventProps) => 
            fetch(`http://localhost:7203/api/events`, {
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