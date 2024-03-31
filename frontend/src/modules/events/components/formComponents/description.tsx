import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventDescription } from "../../store/selectedEventSlice";


const DescriptionField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.description)
            inputRef.current.value = event.description;
    })

    const handleDescriptionChange = (e : any) => {
        const value = e.target.value;
        if(value) {
            updateEvent(event.uuid, '/description', value)
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