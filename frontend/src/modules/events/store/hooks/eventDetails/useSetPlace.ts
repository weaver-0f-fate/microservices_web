import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetPlace = () => {
    const setDraft = useSetDraft();

    return useCallback((place: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.place = place;
        })
    }, [setDraft]);
}

export default useSetPlace;