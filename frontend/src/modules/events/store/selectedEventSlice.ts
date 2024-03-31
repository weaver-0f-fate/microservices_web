import { createSlice } from '@reduxjs/toolkit'
import { Event } from '../../../shared/models/events';

const initialState: Event = {
    uuid: "",
    category: "",
    title: "",
    imageUrl: "",
    description: "",
    place: "",
    date: new Date(),
    additionalInfo: "",  
}

export const selectedEventSlice = createSlice({
  name: 'events',
  initialState,
  reducers: {
    setSelectedEvent: (state: any, action) => {
        state = action.payload;
    },
    setSelectedEventTitle: (state: any, action) => {
        state.title = action.payload;
    },
    setSelectedEventImageUrl: (state: any, action) => {
        state.imageUrl = action.payload;
    },
    setSelectedEventDescription: (state: any, action) => {
        state.description = action.payload;
    },
    setSelectedEventPlace: (state: any, action) => {
        state.place = action.payload;
    },
    setSelectedEventDate: (state: any, action) => {
        state.date = action.payload;
    },
    setSelectedEventAdditionalInfo: (state: any, action) => {
        state.additionalInfo = action.payload;
    },
    setSelectedEventRecurrency: (state: any, action) => {
        state.recurrency = action.payload;
    },
    setSelectedEventCategory: (state: any, action) => {
        state.category = action.payload;
    },
    setSelectedEventUuid: (state: any, action) => {
        state.uuid = action.payload;
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