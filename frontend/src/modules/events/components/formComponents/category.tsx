import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventCategory } from "../../store/selectedEventSlice";

const CategoryField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();    
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();
    
    useEffect(() => {
        if(event.category)
            inputRef.current.value = event.category;
    }, [event.category])

    const handleCategoryChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.uuid, '/category', value)
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