import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetTimeFilter = () => {
    const setDraft = useSetDraft();

    return useCallback((time: Date | undefined) => {
        setDraft((draft: EventsState) => {
            draft.filters.time = time;
        })
    }, [setDraft]);
}

export default useSetTimeFilter;