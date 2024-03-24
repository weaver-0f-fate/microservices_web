import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetCategory from "../../store/hooks/eventDetails/useSetCategory";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";

const CategoryField = () => {
    const eventStore = useEvent();
    const setCategory = useSetCategory();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();
    
    useEffect(() => {
        if(eventStore.selectedEvent.category)
            inputRef.current.value = eventStore.selectedEvent.category;
    }, [eventStore.selectedEvent.category])

    const handleCategoryChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/category', value)
            .then(() => {
                setCategory(value);
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