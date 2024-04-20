import { configureStore,  } from '@reduxjs/toolkit'
import eventsReducer from '../../modules/events/store/eventsSlice'
import selectedEventReducer from '../../modules/events/store/selectedEventSlice'
import filtersReducer from '../../modules/events/store/filtersSlice'
import appConfigReducer from '../store/appConfigSlice'

export default configureStore({
  reducer: {
    events: eventsReducer,
    selectedEvent: selectedEventReducer,
    filters: filtersReducer,
    appConfig: appConfigReducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({
    serializableCheck: false
  })
})