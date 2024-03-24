import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetAdditionalInfo from "../../store/hooks/eventDetails/useSetAdditionalInfo";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";


const AdditionalInfoField = () => {
    const eventStore = useEvent();
    const setAdditionalInfo = useSetAdditionalInfo();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(eventStore.selectedEvent.additionalInfo)
            inputRef.current.value = eventStore.selectedEvent.additionalInfo;
    }, [eventStore.selectedEvent.additionalInfo])

    const handleAdditionalInfoChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/additionalInfo', value)
            .then(() => {
                setAdditionalInfo(value);
            })
            .catch(err => console.log(err));
        }
    }
    
    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="additionalInfo-input">Additional Info</InputLabel>
            <TextField
                id="additionalInfo-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleAdditionalInfoChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default AdditionalInfoField;