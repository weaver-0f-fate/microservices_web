import { configureStore,  } from '@reduxjs/toolkit'
import eventsReducer from '../../modules/events/store/eventsSlice'
import selectedEventReducer from '../../modules/events/store/selectedEventSlice'
import filtersReducer from '../../modules/events/store/filtersSlice'

export default configureStore({
  reducer: {
    events: eventsReducer,
    selectedEvent: selectedEventReducer,
    filters: filtersReducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({
    serializableCheck: false
  })
})