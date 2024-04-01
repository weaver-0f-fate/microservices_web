import { createSlice } from '@reduxjs/toolkit'
import { Event } from '../../../shared/models/events';

interface SelectedEventState {
    event: Event
}

const initialState: SelectedEventState = {
    event: {
        uuid: "",
        category: "",
        title: "",
        imageUrl: "",
        description: "",
        place: "",
        date: new Date(),
        additionalInfo: "",  
    }
}

export const selectedEventSlice = createSlice({
  name: 'events',
  initialState,
  reducers: {
    setSelectedEvent: (state: SelectedEventState, action) => {
        state.event = action.payload;
    },
    setSelectedEventTitle: (state: SelectedEventState, action) => {
        state.event.title = action.payload;
    },
    setSelectedEventImageUrl: (state: SelectedEventState, action) => {
        state.event.imageUrl = action.payload;
    },
    setSelectedEventDescription: (state: SelectedEventState, action) => {
        state.event.description = action.payload;
    },
    setSelectedEventPlace: (state: SelectedEventState, action) => {
        state.event.place = action.payload;
    },
    setSelectedEventDate: (state: SelectedEventState, action) => {
        state.event.date = action.payload;
    },
    setSelectedEventAdditionalInfo: (state: SelectedEventState, action) => {
        state.event.additionalInfo = action.payload;
    },
    setSelectedEventRecurrency: (state: SelectedEventState, action) => {
        state.event.recurrency = action.payload;
    },
    setSelectedEventCategory: (state: SelectedEventState, action) => {
        state.event.category = action.payload;
    },
    setSelectedEventUuid: (state: SelectedEventState, action) => {
        console.log(action.payload)
        console.log(state.event)
        state.event.uuid = action.payload;
    }
  }
})

// Action creators are generated for each case reducer function
export const { 
    setSelectedEvent, 
    setSelectedEventTitle,
    setSelectedEventAdditionalInfo,
    setSelectedEventCategory,
    setSelectedEventDate,
    setSelectedEventDescription,
    setSelectedEventImageUrl,
    setSelectedEventPlace,
    setSelectedEventRecurrency,
    setSelectedEventUuid  } = selectedEventSlice.actions

export default selectedEventSlice.reducer