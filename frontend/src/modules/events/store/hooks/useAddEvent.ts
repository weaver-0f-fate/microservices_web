import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';
import { EventBaseInfo } from '../../eventsCalendar';
import { getLocalDate } from '../../../../shared/utilities/dateFunctions';

const useDeleteEvent = () => {
    const setDraft = useSetDraft();

    return useCallback((eventBase: EventBaseInfo) => {
        setDraft((draft: EventsState) => 
        {   
            eventBase.date = getLocalDate(eventBase.date)
            draft.events.push(eventBase);
        })
    }, [setDraft]);
}

export default useDeleteEvent;