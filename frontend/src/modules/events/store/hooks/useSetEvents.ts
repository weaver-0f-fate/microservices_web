import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';
import { getLocalDate } from '../../../../shared/utilities/dateFunctions';
import { EventResponse } from '../../fetch/useGetEvents';

const useSetEvents = () => {
    const setDraft = useSetDraft();

    return useCallback((events: EventResponse[]) => {
        setDraft((draft: EventsState) => {
            draft.events = events.map(event => {
                return {
                    title: event.title,
                    rrule: "123", //event.rrule,
                    date: getLocalDate(event.date),
                    category: event.category,
                    place: event.place,
                    uuid: event.uuid
                }
            });
        })
    }, [setDraft]);
}

export default useSetEvents;