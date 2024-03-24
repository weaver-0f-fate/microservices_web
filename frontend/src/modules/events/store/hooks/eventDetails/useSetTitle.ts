import { useCallback } from 'react';
import { EventsState, useSetDraft } from '../../store';

const useSetTitle = () => {
    const setDraft = useSetDraft();

    return useCallback((eventUuid: string, title: string) => {
        setDraft((draft: EventsState) => {
            draft.selectedEvent.title = title;
            
            let targetEvent = draft.events.find(event => event.uuid === eventUuid);
            if(targetEvent)
                targetEvent.title = title;
        })
    }, [setDraft]);
}

export default useSetTitle;