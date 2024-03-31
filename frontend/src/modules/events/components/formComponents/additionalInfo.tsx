import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventAdditionalInfo } from "../../store/selectedEventSlice";

const AdditionalInfoField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.additionalInfo)
            inputRef.current.value = event.additionalInfo;
    }, [event.additionalInfo])

    const handleAdditionalInfoChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.uuid, '/additionalInfo', value)
            .then(() => {
                dispatch(setSelectedEventAdditionalInfo(value));
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