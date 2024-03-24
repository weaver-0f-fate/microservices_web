import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetCategoryFilter = () => {
    const setDraft = useSetDraft();

    return useCallback((category: string) => {
        setDraft((draft: EventsState) => {
            draft.filters.category = category;
        })
    }, [setDraft]);
}

export default useSetCategoryFilter;