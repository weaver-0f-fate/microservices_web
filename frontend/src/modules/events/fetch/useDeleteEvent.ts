import { useCallback } from "react";
import { usePublicFetch } from "../../../shared/fetch/use-fetch";

export const useDeleteEvent = () => {
    const fetch = usePublicFetch();

    return useCallback(
        (eventUuid: string) : Promise<void> => 
            fetch(`http://localhost:7201/api/events/${eventUuid}`, {
                method: 'DELETE',
        }), [])
}