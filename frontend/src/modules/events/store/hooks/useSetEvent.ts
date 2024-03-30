import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';
import { getLocalDate } from '../../../../shared/utilities/dateFunctions';
import { EventResponse } from '../../fetch/useGetEvent';

const useSetEvent = () => {
    const setDraft = useSetDraft();

    return useCallback((event: EventResponse) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.category = event.category;
            draft.selectedEvent.title = event.title;
            draft.selectedEvent.imageUrl = event.imageUrl;
            draft.selectedEvent.description = event.description;
            draft.selectedEvent.place = event.place;
            draft.selectedEvent.date = event.date ? getLocalDate(event.date) : new Date();
            draft.selectedEvent.additionalInfo = event.additionalInfo;
            draft.selectedEvent.recurrency = event.recurrency;
        })
    }, [setDraft]);
}

export default useSetEvent;