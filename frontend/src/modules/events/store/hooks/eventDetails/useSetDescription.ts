import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetDescription = () => {
    const setDraft = useSetDraft();

    return useCallback((description: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.description = description;
        })
    }, [setDraft]);
}

export default useSetDescription;