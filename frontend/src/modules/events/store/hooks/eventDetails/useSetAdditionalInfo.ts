import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetAdditionalInfo = () => {
    const setDraft = useSetDraft();

    return useCallback((additionalInfo: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.additionalInfo = additionalInfo;
        })
    }, [setDraft]);
}

export default useSetAdditionalInfo;