import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetEventUuid = () => {
    const setDraft = useSetDraft();

    return useCallback((eventUuid: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.uuid = eventUuid;
        })
    }, [setDraft]);
}

export default useSetEventUuid;