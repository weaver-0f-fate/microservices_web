import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../store';
import { EventBaseInfo } from '../../eventsCalendar';
import { getLocalDate } from '../../../../shared/utilities/dateFunctions';

const useSetEvents = () => {
    const setDraft = useSetDraft();

    return useCallback((events: EventBaseInfo[]) => {
        setDraft((draft: EventsState) => {
            draft.events = events.map(event => {
                return {
                    title: event.title,
                    rrule: event.rrule,
                    date: getLocalDate(event.date),
                    category: event.category,
                    place: event.place,
                    uuid: event.uuid,
                }
            });
        })
    }, [setDraft]);
}

export default useSetEvents;