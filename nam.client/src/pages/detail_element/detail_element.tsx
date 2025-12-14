import {
  Container,
  Typography,
  Box,
  Divider,
  List,
  ListItem,
  Chip,
  ListItemText,
  Stack,
  ImageList,
  ImageListItem,
  CardMedia,
  Card,
  CardActionArea,
  CardContent,
  Grid,
} from "@mui/material";

import {
  Event,
  LocationOn,
  Email,
  Phone,
  Image,
  Paid,
  GroupWork,
  Info,
  AccessTime,
  LinkOutlined,
  WorkRounded,
  Facebook,
  Instagram,
  ContactPage,
  LocationSearching,
  Timer,
  Description,
} from "@mui/icons-material";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useLocationEvent } from "./hooks/useDetailEvents";
import MyAppBar from "../../components/appbar";
import { CategoryApi, stringToCategoryAPI } from "./hooks/IDetailElement";
import StatusCard from "../../components/status_screen";
import SectionHeader from "./component/section_header";
import { DetailRow } from "./component/detail_row";
import { loadingView } from "../../components/loading";
import { buildApiUrl } from "../../config";

//TODO: replace when migration complete
const BASE_URL_IMG = buildApiUrl("image/external?imagePath=");
interface EventState {
  id: string;
  category: CategoryApi;
}

export function EventDetail() {
  const location = useLocation();
  const eventState = location.state as EventState | undefined;

  const { data, loading, error } = useLocationEvent(
    eventState?.id ?? "",
    eventState?.category ?? CategoryApi.ARTICLE
  );
  const navigate = useNavigate();
  if (loading) return loadingView;
  if (error)
    return (
      <StatusCard
        type="error"
        title="🚫 Errore"
        message={error}
        buttonText="Back"
        onAction={() => {
          navigate("/maincontents");
        }}
      />
    );
  if (!data)
    return (
      <StatusCard
        type="empty"
        title="🚫 Not Found"
        message="Data not found"
        buttonText="Back"
        onAction={() => {
          navigate("/maincontents");
        }}
      />
    );

  const renderSubtitle = () =>
    data.subtitle == undefined ? null : (
      <Typography>{data.subtitle}</Typography>
    );

  const renderScript = () =>
    data.script == undefined ? null : (
      <Typography variant="body2">{data.script}</Typography>
    );

  const renderMainBadgeInformation = () => {
    if (
      data.timeToRead == undefined &&
      data.updatedAt == undefined &&
      data.type == undefined
    )
      return null;
    return (
      <Stack direction="row" spacing={1} sx={{ p: 1 }}>
        {data.timeToRead && (
          <Chip
            icon={<AccessTime />}
            label={`Read: ${data.timeToRead}`}
            color="primary"
            variant="outlined"
            size="small"
          />
        )}
        {data.updatedAt && (
          <Chip
            icon={<Timer />}
            label={`last update: ${new Date(data.updatedAt).toLocaleString(
              "en-EN"
            )}`}
            color="primary"
            variant="outlined"
            size="small"
          />
        )}
        {data.type && (
          <Chip
            label={data.type}
            color="primary"
            variant="outlined"
            size="small"
          />
        )}
      </Stack>
    );
  };
  function cleanPath(referenceImagePath: string) {
    return referenceImagePath
      .replace(/-thumb-/g, "-")
      .replace(/-thumb(?=\.[^.]+$)/, "");
  }
  const renderParagraphArticle = () => {
    if ((data.paragraphs ?? []).length < 1) return null;

    return (
      <Box className="story-content" sx={{ mb: 3 }}>
        {(data.paragraphs ?? [])
          .sort((a, b) => a.position - b.position)
          .map((paragraph) => (
            <Container key={paragraph.position}>
              {paragraph.title && (
                <Typography variant="h3">{paragraph.title}</Typography>
              )}
              {paragraph.subtitle && (
                <Typography variant="subtitle1">
                  {paragraph.subtitle}
                </Typography>
              )}

              {paragraph.referenceImagePath && (
                <CardMedia
                  component="img"
                  sx={{
                    height: 140,
                    borderRadius: 1,
                    objectFit: "cover",
                    mt: 1,
                  }}
                  image={buildApiUrl(
                    "image/external?imagePath=" +
                      cleanPath(paragraph.referenceImagePath)
                  )}
                  alt={`Image reference for ${paragraph.referenceName}`}
                />
              )}
              <Typography variant="body2">{paragraph.script}</Typography>
              {paragraph.referenceName && (
                <Box
                  className="reference-box"
                  sx={{
                    borderLeft: "4px solid",
                    borderColor: "primary.main",
                    p: 2,
                    bgcolor: "action.hover",
                    mt: 2,
                    borderRadius: 1,
                  }}
                >
                  <Stack direction="row" alignItems="center" spacing={1}>
                    <Typography
                      variant="subtitle2"
                      sx={{ fontWeight: "bold", color: "primary.dark" }}
                    >
                      <LinkOutlined
                        fontSize="small"
                        sx={{ verticalAlign: "middle", mr: 0.5 }}
                      />
                      Reference
                    </Typography>

                    <Typography variant="body1">
                      {paragraph.referenceName}
                    </Typography>
                    {paragraph.referenceCategory && (
                      <Chip
                        label={paragraph.referenceCategory}
                        size="small"
                        color="secondary"
                        variant="outlined"
                        sx={{ ml: 1, height: 20 }}
                      />
                    )}
                  </Stack>
                </Box>
              )}
              <Divider sx={{ my: 2 }} light />
            </Container>
          ))}
      </Box>
    );
  };

  const renderMainDetails = () => {
    if (
      data.typology == undefined &&
      data.audience == undefined &&
      data.startDate == undefined &&
      data.endDate == undefined
    )
      return null;
    {
      return (
        <Stack
          direction="row"
          justifyContent="space-between"
          alignItems="center"
          mb={3}
          spacing={2}
        >
          <Stack direction="column" alignItems="flex-start" spacing={1}>
            {data.typology && (
              <Chip
                label={`Type: ${data.typology}`}
                color="primary"
                variant="outlined"
                size="small"
              />
            )}
            {data.audience && (
              <Chip
                label={`Audience: ${data.audience}`}
                color="secondary"
                variant="outlined"
                size="small"
              />
            )}
          </Stack>
          {(data.startDate || data.endDate) && (
            <Stack direction="row" alignItems="center" spacing={1}>
              <Event color="primary" />
              <Stack direction="column" alignItems="flex-end" spacing={0.5}>
                {data.startDate && (
                  <Typography variant="body2">
                    <strong>Start</strong> {data.startDate}
                  </Typography>
                )}
                {data.endDate && (
                  <Typography variant="body2" color="text.secondary">
                    <strong>End</strong> {data.endDate}
                  </Typography>
                )}
              </Stack>
            </Stack>
          )}
          <Divider sx={{ mb: 2 }} />
        </Stack>
      );
    }
  };
  const renderAddress = () => {
    if (
      (data.address ?? []).length < 1 &&
      (data.latitude ?? 0) <= 0 &&
      (data.longitude ?? 0) <= 0
    )
      return null;
    return (
      <Box sx={{ textAlign: "center", mb: 3 }}>
        <Stack
          direction="row"
          alignItems="center"
          justifyContent="center"
          spacing={1}
        >
          <LocationOn color="error" />
          <Typography
            variant="subtitle1"
            component="h2"
            sx={{ fontWeight: "bold", textTransform: "uppercase" }}
          ></Typography>
        </Stack>
        {data.latitude !== undefined && data.longitude !== undefined && (
          <Typography variant="caption" color="text.secondary">
            ({data.latitude}, {data.longitude})
          </Typography>
        )}
        <Divider sx={{ mb: 3 }} />
      </Box>
    );
  };
  const renderDescription = () => {
    if ((data.description ?? "").length < 1) return null;
    return (
      <Box sx={{ mb: 3 }}>
        <SectionHeader title={"Description"} IconComponent={Description} />
        <Typography
          variant="body2"
          component="p"
          sx={{ whiteSpace: "pre-line", lineHeight: 1.6 }}
        >
          {data.description}
        </Typography>
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };
  const renderingSectionAndTicket = () => {
    if ((data.ticketsAndCosts ?? []).length < 1) return null;
    return (
      <Box sx={{ mb: 5 }}>
        <SectionHeader title={"Partecipate"} />
        <List disablePadding>
          {(data.ticketsAndCosts ?? []).map((ticket, i) => (
            <ListItem
              id={`ticket-${i}`}
              disableGutters
              secondaryAction={
                <Chip
                  icon={<Paid />}
                  label={`${ticket.priceSpecificationCurrencyValue} ${ticket.currency}`}
                  color="success"
                  variant="filled"
                  sx={{ fontWeight: "bold" }}
                />
              }
            >
              <ListItemText
                primary={
                  <Typography
                    variant="body1"
                    component="p"
                    sx={{ fontWeight: "bold" }}
                  >
                    {ticket.description}
                  </Typography>
                }
                secondary={
                  <Typography variant="body2" color="text.secondary">
                    {ticket.validityDescription}
                  </Typography>
                }
              />
            </ListItem>
          ))}
        </List>
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingServices = () => {
    if ((data.services ?? []).length < 1) return null;
    return (
      <Box>
        <SectionHeader title="Services" IconComponent={Info} />
        <List disablePadding>
          {(data.services ?? []).map((service, i) => (
            <ListItem disableGutters key={`service-${i}`}>
              {service.imagePath && (
                <CardMedia
                  component="img"
                  image={`${BASE_URL_IMG}${cleanPath(service.imagePath)}`}
                  alt={service.name ?? "Service image"}
                  sx={{
                    width: 60,
                    height: 60,
                    borderRadius: 1,
                    mr: 2,
                    objectFit: "cover",
                  }}
                />
              )}
              <ListItemText
                primary={
                  <Typography
                    variant="body1"
                    component="p"
                    sx={{ fontWeight: "bold" }}
                  >
                    {service.name}
                    {service.identifier && (
                      <Chip
                        label={service.identifier}
                        size="small"
                        color="primary"
                        variant="outlined"
                        sx={{ ml: 1 }}
                      />
                    )}
                  </Typography>
                }
                secondary={
                  service.description && (
                    <Typography variant="body2" color="text.secondary">
                      {service.description}
                    </Typography>
                  )
                }
              />
            </ListItem>
          ))}
        </List>
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingContact = () => {
    if (
      (data.email ?? "").length == 0 &&
      (data.telephone ?? "").length == 0 &&
      (data.website ?? "").length == 0
    )
      return null;
    return (
      <Box sx={{ mb: 5 }}>
        <SectionHeader title="Contact" IconComponent={ContactPage} />
        {data.email && (
          <DetailRow
            label={"Email:"}
            value={data.email}
            isLink={true}
            IconComponent={Email}
          />
        )}

        {data.telephone && (
          <DetailRow
            label={"Phone:"}
            value={data.telephone}
            IconComponent={Phone}
          />
        )}

        {data.website && (
          <DetailRow
            label={"Website:"}
            value={data.website}
            isLink={true}
            IconComponent={WorkRounded}
          />
        )}
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingOrganizer = () => {
    if (
      (data.organizer?.legalName ?? "").length == 0 &&
      (data.organizer?.website ?? "").length == 0 &&
      (data.facebook ?? "").length == 0 &&
      (data.instagram ?? "").length == 0
    )
      return null;

    return (
      <Box sx={{ mb: 5 }}>
        <SectionHeader title="Organizer" IconComponent={GroupWork} />
        {data.organizer?.legalName && (
          <DetailRow label={"Name"} value={data.organizer.legalName} />
        )}
        {data.organizer?.website && (
          <DetailRow
            label={"Website"}
            value={data.organizer.website}
            isLink={true}
          />
        )}
        {data.facebook && (
          <DetailRow
            label={"Facebook:"}
            value={data.facebook}
            isLink={true}
            IconComponent={Facebook}
          />
        )}
        {data.instagram && (
          <DetailRow
            label={"Instagram:"}
            value={data.instagram}
            isLink={true}
            IconComponent={Instagram}
          />
        )}
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingCreativeWork = () => {
    if (!data?.creativeWorks?.length) return null;

    return (
      <Box sx={{ p: 2 }}>
        <SectionHeader title="Creative Works" IconComponent={WorkRounded} />

        <List dense>
          {data.creativeWorks.map((work) => (
            <ListItem key={work.url} sx={{ px: 2, py: 1 }}>
              <ListItemText
                primary={
                  <Typography
                    variant="subtitle2"
                    component="span"
                    sx={{ fontWeight: "bold" }}
                  >
                    {work.type}:
                  </Typography>
                }
                secondary={
                  <Link
                    to={work.url}
                    target="_blank"
                    rel="noopener noreferrer"
                    color="primary"
                    style={{
                      display: "block",
                      whiteSpace: "normal",
                      wordBreak: "break-word",
                      overflowWrap: "anywhere",
                    }}
                  >
                    {work.url}
                  </Link>
                }
                sx={{ mb: 0.5 }}
              />
            </ListItem>
          ))}
        </List>

        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingOffers = () => {
    if ((data.offers ?? []).length < 1) return null;
    return (
      <Box sx={{ mb: 5 }}>
        <SectionHeader title="Offers" IconComponent={Paid} />
        <List disablePadding>
          {(data.offers ?? []).map((offer, i) => (
            <ListItem key={`offer-${i}`} disableGutters>
              <ListItemText
                primary={
                  <Stack direction="row" alignItems="center" spacing={1}>
                    <Typography variant="body1" sx={{ fontWeight: "bold" }}>
                      {offer.description}
                    </Typography>
                    <Chip
                      label={`${offer.priceSpecificationCurrencyValue} ${offer.currency}`}
                      color="success"
                      variant="filled"
                      size="small"
                      sx={{ fontWeight: "bold" }}
                    />
                  </Stack>
                }
                secondary={
                  <Box>
                    {offer.validityDescription && (
                      <Typography variant="body2" color="text.secondary">
                        {offer.validityDescription}
                      </Typography>
                    )}
                    {(offer.validityStartDate || offer.validityEndDate) && (
                      <Typography variant="caption" color="text.secondary">
                        {offer.validityStartDate &&
                          `Valid from: ${new Date(
                            offer.validityStartDate
                          ).toLocaleDateString()}`}
                        {offer.validityEndDate &&
                          ` to ${new Date(
                            offer.validityEndDate
                          ).toLocaleDateString()}`}
                      </Typography>
                    )}
                    {offer.userTypeName && (
                      <Typography variant="body2" color="primary">
                        {offer.userTypeName}
                      </Typography>
                    )}
                    {offer.userTypeDescription && (
                      <Typography variant="caption" color="text.secondary">
                        {offer.userTypeDescription}
                      </Typography>
                    )}
                    {offer.ticketDescription && (
                      <Typography variant="body2" color="text.secondary">
                        {offer.ticketDescription}
                      </Typography>
                    )}
                  </Box>
                }
              />
            </ListItem>
          ))}
        </List>
        <Divider sx={{ my: 3 }} />
      </Box>
    );
  };

  const renderingGallery = () => {
    if ((data.gallery ?? []).length < 1) return null;
    return (
      <Box sx={{ mb: 5 }}>
        <SectionHeader title="Gallery" IconComponent={Image} />

        <ImageList cols={2} gap={8}>
          {(data.gallery ?? []).map((img, i) => (
            <ImageListItem
              key={
                img
                  .substring(img.lastIndexOf("/") + 1)
                  .replace("primary-", "")
                  .split(".")[0]
              }
            >
              <img
                srcSet={`${BASE_URL_IMG}${cleanPath(img)}`}
                src={`${BASE_URL_IMG}${cleanPath(img)}`}
                alt={`Gallery Image ${i + 1}`}
                loading="lazy"
                style={{ borderRadius: 8 }}
              />
            </ImageListItem>
          ))}
        </ImageList>
      </Box>
    );
  };

  const renderingNeighbors = () => {
    if ((data.neighbors ?? []).length < 1) return null;

    return (
      <Box>
        <SectionHeader
          title={"Nearby Points of Interest"}
          IconComponent={LocationSearching}
        />

        <Grid container spacing={3}>
          {(data.neighbors ?? []).map((neighbor) => (
            <Card sx={{ height: "100%", width: "100%" }}>
              <CardActionArea
                onClick={() =>
                  navigate("/detail-element", {
                    state: {
                      id: neighbor.entityId,
                      category: stringToCategoryAPI(neighbor.category),
                    },
                  })
                }
              >
                <CardMedia
                  component="img"
                  height="140"
                  image={`${BASE_URL_IMG}${cleanPath(neighbor.imagePath)}`}
                  alt={`Image of ${neighbor.title}`}
                  sx={{ objectFit: "cover" }}
                />
                <CardContent>
                  <Typography
                    gutterBottom
                    variant="h6"
                    component="div"
                    sx={{ lineHeight: 1.4 }}
                  >
                    {neighbor.title}
                  </Typography>
                  <Box
                    sx={{
                      mt: 1,
                      display: "flex",
                      gap: 1,
                      flexWrap: "wrap",
                    }}
                  >
                    <Chip
                      label={neighbor.category}
                      size="small"
                      color="primary"
                      variant="outlined"
                    />
                    {neighbor.extraInfo && (
                      <Chip
                        label={`Distance: ${neighbor.extraInfo}`}
                        size="small"
                        color="secondary"
                      />
                    )}
                  </Box>
                </CardContent>
              </CardActionArea>
            </Card>
          ))}
        </Grid>
      </Box>
    );
  };

  return (
    <Container maxWidth="lg">
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          paddingTop: 1,
          paddingBottom: 4,
        }}
      >
        <MyAppBar title={data.title ?? data.officialName ?? "Details"} back />
        <Box sx={{ p: 2 }} />
        {renderSubtitle()}
        {renderScript()}
        {renderMainBadgeInformation()}
        {renderParagraphArticle()}
        {renderMainDetails()}
        {renderAddress()}
        {renderDescription()}
        {renderingSectionAndTicket()}
        {renderingServices()}
        {renderingContact()}
        {renderingOrganizer()}
        {renderingCreativeWork()}
        {renderingOffers()}
        {renderingGallery()}
        {renderingNeighbors()}
      </Box>
    </Container>
  );
}
