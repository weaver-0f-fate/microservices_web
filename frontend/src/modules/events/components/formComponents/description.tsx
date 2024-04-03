import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { SelectedEventState, setSelectedEventDescription } from "../../store/selectedEventSlice";


const DescriptionField = () => {
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();
    const disabled = event.event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.event.description)
            inputRef.current.value = event.event.description;
    })

    const handleDescriptionChange = (e : any) => {
        const value = e.target.value;
        if(value) {
            updateEvent(event.event.uuid, '/description', value)
            .then(() => {
                dispatch(setSelectedEventDescription(value));
            })
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="description-input">Description</InputLabel>
            <TextField
                id="description-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleDescriptionChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default DescriptionField;