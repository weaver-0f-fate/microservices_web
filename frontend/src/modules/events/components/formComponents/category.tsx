import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { SelectedEventState, setSelectedEventCategory } from "../../store/selectedEventSlice";

const CategoryField = () => {
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();    
    const disabled = event.event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();
    
    useEffect(() => {
        if(event.event.category)
            inputRef.current.value = event.event.category;
    }, [event])

    const handleCategoryChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.event.uuid, '/category', value)
            .then(() => {
                dispatch(setSelectedEventCategory(value));
            })
            .catch(err => console.log(err));
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="category-input">Category</InputLabel>
            <TextField
                id="category-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleCategoryChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default CategoryField;