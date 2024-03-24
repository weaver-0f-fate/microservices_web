import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetCategory = () => {
    const setDraft = useSetDraft();

    return useCallback((category: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.category = category;
        })
    }, [setDraft]);
}

export default useSetCategory;