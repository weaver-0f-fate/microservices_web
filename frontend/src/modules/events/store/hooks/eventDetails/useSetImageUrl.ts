import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetImageUrl = () => {
    const setDraft = useSetDraft();

    return useCallback((imageUrl: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.imageUrl = imageUrl;
        })
    }, [setDraft]);
}

export default useSetImageUrl;