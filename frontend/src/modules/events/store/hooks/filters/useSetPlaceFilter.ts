import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetPlaceFilter = () => {
    const setDraft = useSetDraft();

    return useCallback((place: string) => {
        setDraft((draft: EventsState) => {
            draft.filters.place = place;
        })
    }, [setDraft]);
}

export default useSetPlaceFilter;