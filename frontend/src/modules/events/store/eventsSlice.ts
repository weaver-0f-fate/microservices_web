import { createSlice } from '@reduxjs/toolkit'
import { EventBaseInfo } from '../eventsCalendar'

const initialState: EventBaseInfo[] = []

export const eventsSlice = createSlice({
  name: 'events',
  initialState: initialState,
  reducers: {
    setEvents: (state: EventBaseInfo[], action) => {
        state = action.payload;
    },
    addEvent: (state: EventBaseInfo[], action) => {
        state.push(action.payload);
    },
    removeEvent: (state: EventBaseInfo[], action) => {
        const index = state.findIndex(event => event.uuid === action.payload);
        state.splice(index, 1);
    },
    setEventTitle: (state: EventBaseInfo[], action) => {
        let target = state.find(event => event.uuid === action.payload.eventUuid);
        if(target)
            target.title = action.payload.title;
    },
    setEventDate: (state: EventBaseInfo[], action) => {
        let target = state.find(event => event.uuid === action.payload.eventUuid);
        if(target)
            target.date = action.payload.date;
    },
    setEventRecurrency: (state: EventBaseInfo[], action) => {
        let target = state.find(event => event.uuid === action.payload.eventUuid);
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