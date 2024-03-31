import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventPlace } from "../../store/selectedEventSlice";


const PlaceField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.place)
            inputRef.current.value = event.place;
    })

    const handlePlaceChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.uuid, '/place', value)
            .then(() => {
                dispatch(setSelectedEventPlace(value));
            })
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="place-input">Place</InputLabel>
            <TextField
                id="place-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handlePlaceChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default PlaceField;