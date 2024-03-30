import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';
import { EventBaseInfo } from '../../eventsCalendar';

const useDeleteEvent = () => {
    const setDraft = useSetDraft();

    return useCallback((eventBase: EventBaseInfo) => {
        setDraft((draft: EventsState) => 
        {
            draft.events.push(eventBase);
        })
    }, [setDraft]);
}

export default useDeleteEvent;