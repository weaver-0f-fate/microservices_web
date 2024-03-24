import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetDescription from "../../store/hooks/eventDetails/useSetDescription";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";


const DescriptionField = () => {
    const eventStore = useEvent();
    const setDescription = useSetDescription();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(eventStore.selectedEvent.description)
            inputRef.current.value = eventStore.selectedEvent.description;
    })

    const handleDescriptionChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            
            updateEvent(eventStore.selectedEvent.uuid, '/description', value)
            .then(() => {
                setDescription(value);
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