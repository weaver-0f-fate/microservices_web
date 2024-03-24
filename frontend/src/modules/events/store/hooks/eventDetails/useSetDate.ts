import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetDate = () => {
    const setDraft = useSetDraft();

    return useCallback((eventUuid:string, date: Date) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.date = date;

            let targetEvent = draft.events.find(event => event.uuid === eventUuid);
            if(targetEvent)
                targetEvent.date = date;
        })
    }, [setDraft]);
}

export default useSetDate;