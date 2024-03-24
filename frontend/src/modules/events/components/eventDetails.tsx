import { Box, Container, Grid } from '@mui/material';
import React, { useEffect } from 'react';
import useEvent from '../store/hooks/useEvent';
import useSetEvent from '../store/hooks/useSetEvent';
import TitleField from './formComponents/title';
import ImageUrlField from './formComponents/imageUrl';
import PlaceField from './formComponents/place';
import AdditionalInfoField from './formComponents/additionalInfo';
import DescriptionField from './formComponents/description';
import DateField from './formComponents/date';
import CategoryField from './formComponents/category';
import { useGetEvent } from '../fetch/useGetEvent';

const EventDetails = () => {
    const eventStore = useEvent();
    const setEvent = useSetEvent();
    const getEvent = useGetEvent();

    useEffect(() => {
        if(eventStore.selectedEvent) {
            getEvent(eventStore.selectedEvent.uuid)
                .then(result => result.json())
                .then(data => {
                    setEvent(data);
                })
        }
    }, [eventStore.selectedEvent.uuid])

    return (
        <Box>
            <Container maxWidth="sm" sx={{ p: 5 }}>
                <Grid container spacing={2}>
                    <CategoryField />
                    <TitleField />
                    <ImageUrlField />
                    <PlaceField />
                    <DescriptionField />
                    <AdditionalInfoField />
                    <DateField />
                </Grid>
            </Container>
        </Box>
    )
}



export default EventDetails;