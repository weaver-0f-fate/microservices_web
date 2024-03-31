import { createSlice } from '@reduxjs/toolkit'

export interface Filters {
    category: string,
    place: string,
    time?: Date,
}

const initialState: Filters = {
    category: "",
    place: ""
}

export const filtersSlice = createSlice({
  name: 'events',
  initialState,
  reducers: {
    setCategoryFilter: (state: any, action) => {
        state.category = action.payload;
    },
    setPlaceFilter: (state: any, action) => {
        state.place = action.payload;
    },
    setTimeFilter: (state: any, action) => {
        state.time = action.payload;
    }
  }
})

// Action creators are generated for each case reducer function
export const { setCategoryFilter, setPlaceFilter, setTimeFilter } = filtersSlice.actions

export default filtersSlice.reducer