import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';

const useDeleteEvent = () => {
    const setDraft = useSetDraft();

    return useCallback((eventUuid: string) => {
        setDraft((draft: EventsState) => {
            let targetEvent = draft.events.find(event => event.uuid === eventUuid);

            if(targetEvent){
                draft.events.splice(draft.events.indexOf(targetEvent), 1);
            }

            draft.selectedEvent = {
                uuid: "",
                category: "",
                title: "",
                imageUrl: "",
                description: "",
                place: "",
                date: new Date(),
                additionalInfo: "",
            };
        })
    }, [setDraft]);
}

export default useDeleteEvent;