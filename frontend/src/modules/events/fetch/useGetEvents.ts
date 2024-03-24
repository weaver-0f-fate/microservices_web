import { useCallback } from "react"
import { formatTime } from "../../../shared/utilities/dateFunctions"

export const useGetEvents = () => {
    return useCallback(
        (category?: string, place?: string, date?: Date) => 
            fetch(`http://localhost:7201/api/events?category=${category}&place=${place}&time=${date ? formatTime(date) : ''}`, {
                method: 'GET',
        }), [])
}