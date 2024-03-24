import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetRecurrency = () => {
    const setDraft = useSetDraft();

    return useCallback((eventUuid: string, recurrency?: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.recurrency = recurrency;

            let targetEvent = draft.events.find(event => event.uuid === eventUuid);
            if(targetEvent)
                targetEvent.rrule = recurrency;
        })
    }, [setDraft]);
}

export default useSetRecurrency;