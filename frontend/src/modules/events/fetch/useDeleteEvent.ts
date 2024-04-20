import { useCallback } from "react";
import { usePublicFetch } from "../../../shared/fetch/use-fetch";

export const useDeleteEvent = () => {
    const fetch = usePublicFetch();

    return useCallback(
        (eventUuid: string) : Promise<void> => 
            fetch(`/events/${eventUuid}`, {
                method: 'DELETE',
        }), [])
}