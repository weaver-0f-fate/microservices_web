import { createSlice } from '@reduxjs/toolkit'
import { EventBaseInfo } from '../eventsCalendar'

interface EventsState  {
    events: EventBaseInfo[]
}

const initialState: EventsState = {
    events: []
}

export const eventsSlice = createSlice({
  name: 'events',
  initialState: initialState,
  reducers: {
    setEvents: (state: EventsState, action) => {
        state.events = action.payload;
    },
    addEvent: (state: EventsState, action) => {
        state.events.push(action.payload);
    },
    removeEvent: (state: EventsState, action) => {
        const index = state.events.findIndex(event => event.uuid === action.payload);
        state.events.splice(index, 1);
    },
    setEventTitle: (state: EventsState, action) => {
        let target = state.events.find(event => event.uuid === action.payload.eventUuid);
        if(target)
            target.title = action.payload.title;
    },
    setEventDate: (state: EventsState, action) => {
        let target = state.events.find(event => event.uuid === action.payload.eventUuid);
        if(target)
            target.date = action.payload.date;
    },
    setEventRecurrency: (state: EventsState, action) => {
        let target = state.events.find(event => event.uuid === action.payload.eventUuid);
        if(target)
            target.rrule = action.payload.recurrency;
    },
  }
})

// Action creators are generated for each case reducer function
export const { 
    setEvents, 
    addEvent, 
    removeEvent,
    setEventDate,
    setEventRecurrency,
    setEventTitle } = eventsSlice.actions

export default eventsSlice.reducer