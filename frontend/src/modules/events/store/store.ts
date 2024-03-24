import { useState, useCallback } from 'react';
import { createContainer } from 'react-tracked';
import produce, { Draft } from 'immer';
import { Event } from '../../../shared/models/events';
import { EventBaseInfo } from '../eventsCalendar';

export interface EventsState {
    events: EventBaseInfo[],
    selectedEvent: Event,
    filters: Filters,
}

export interface Filters {
    category: string,
    place: string,
    time?: Date,
}

const initialState: EventsState = {
    events: [],
    selectedEvent: {
        uuid: "",
        category: "",
        title: "",
        imageUrl: "",
        description: "",
        place: "",
        date: new Date(),
        additionalInfo: "",  
    } ,
    filters: {
        category: "",
        place: "",
    }
}

const useValue = () => useState(initialState);

const { Provider, useTrackedState, useUpdate: useSetState } = createContainer(
    useValue
);

const useSetDraft = () => {
    const setState = useSetState();
    return useCallback(
        (draftUpdater: (draft: Draft<EventsState>) => void) => {
            setState(produce(draftUpdater));
        },
        [setState]
    );
};

export { Provider, useTrackedState, useSetDraft, initialState };