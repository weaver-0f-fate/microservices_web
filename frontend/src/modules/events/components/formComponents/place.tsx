import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetPlace from "../../store/hooks/eventDetails/useSetPlace";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";


const PlaceField = () => {
    const eventStore = useEvent();
    const setPlace = useSetPlace();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(eventStore.selectedEvent.place)
            inputRef.current.value = eventStore.selectedEvent.place;
    })

    const handlePlaceChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/place', value)
            .then(() => {
                setPlace(value);
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